using System;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using Microsoft.Bot.Builder.Luis;
using Microsoft.Bot.Builder.Luis.Models;
using System.Linq;
using SharePointHelperBOT.DAL;
using SharePointHelperBOT.Models;
using System.Collections.Generic;

namespace SharePointHelperBOT.Dialogs
{
    [LuisModel("904ed409-bb5c-406e-9898-b906d0b40165", "080ce8f121274030a628af6a2b940390")]
    [Serializable]
    public class SharePointHelperDialog : LuisDialog<object>
    {
        private List<FAQ> _allFAQs;
       
        public SharePointHelperDialog(ILuisService service)
            : base(service)
        {
        }

        public SharePointHelperDialog(List<FAQ> faqs)
        {
            _allFAQs = faqs;
        }

        //Define entites as constants
        public const string Entity_Data_Structure = "data-structure";
        public const string Entity_Action = "action";
        public const string Entity_BuiltIn_Datetime = "builtin.datetime.date";
        public const string Entity_BuiltTn_Email = "email";
        public const string Entity_Location = "location";
        public const string Entity_BuiltTn_Number = "builtin.number";
        public const string Entity_BuiltTn_Ordinal = "ordinal";
        public const string Entity_BuiltTn_Percentage = "percentage";
        public const string Entity_Platform = "platform";
        public const string Entity_Subject = "subject";
        public const string Entity_Task = "task";
        public const string Entity_BuiltTn_Url = "url";
        public const string Entity_CellPhone = "cell phone";
        public const string Entity_Create = "create";


        [LuisIntent("")]
        public async Task None(IDialogContext context, LuisResult result)
        {
            //string message = $"Sorry I did not understand: " + string.Join(", ", result.Intents.Select(i => i.Intent));
            string message = "Sorry I did not understand.";
            await context.PostAsync(message);
            context.Wait(MessageReceived);
        }

        [LuisIntent("create list")]
        private async Task CreateDataStructure(IDialogContext context, LuisResult result)
        {
            #region ExtractingEntites
            EntityRecommendation ds;
            if (!result.TryFindEntity(Entity_Data_Structure, out ds)) {
                ds = null;
            }
            EntityRecommendation task;
            if (!result.TryFindEntity(Entity_Task, out task))
            {
                task = null;
            }
            EntityRecommendation subject;
            if (!result.TryFindEntity(Entity_Subject, out subject))
            {
                subject = null;
            }
            EntityRecommendation platform;
            if (!result.TryFindEntity(Entity_Platform, out platform))
            {
                platform = null;
            }
            EntityRecommendation location;
            if (!result.TryFindEntity(Entity_Location, out location))
            {
                location = null;
            }
            EntityRecommendation cellphone;
            if (!result.TryFindEntity(Entity_CellPhone, out cellphone))
            {
                cellphone = null;
            }
            EntityRecommendation create;
            if (!result.TryFindEntity(Entity_Create, out create))
            {
                create = null;
            }
            #endregion

            var taskEntity = task != null ? task.Entity : null;
            var subEntity = subject != null ? subject.Entity : null;
            var platformEntity = platform != null ? platform.Entity : null;
            var locationEntity = location != null ? location.Entity : null;
            var cellphoneEntity = cellphone != null ? cellphone.Type : null;
            var createEntity = create != null ? create.Type : null;
            createEntity = taskEntity != null ? taskEntity : createEntity;
            var dataStructureName = ds != null ? ds.Entity : null;
            var reply = context.MakeMessage();
            //get the reply from database
            var faqEntity = _allFAQs.Where(x => x.Task == createEntity)
                                 .Where(x => x.Subject == subEntity)
                                 .Where(x => x.DataStructure == dataStructureName)
                                 .Where(x => x.Platform == platformEntity || x.Platform == cellphoneEntity)
                                 .Where(x => x.Location == locationEntity).FirstOrDefault();
            var answer = faqEntity != null ? faqEntity.Answer : "I dont find anything in the DB";

            reply.Text = $"Here is your answer : {answer}";
            await context.PostAsync(reply);

            var answerClass = faqEntity != null ? faqEntity.Classification : "-";
            reply.Text = $"Classification : {answerClass}";
            await context.PostAsync(reply);

            reply.Text = "Please login in using this promt";
            reply.Attachments.Add(SigninCard.Create("You need to authorize me",
                                                    "Login to Office 365!",
                                                    "https://login.microsoft.com"
                                                    ).ToAttachment());
            await context.PostAsync(reply);

            context.Wait(MessageReceived);
        }

        [LuisIntent("welcome")]
        private async Task SendWelcomeMessage(IDialogContext context, LuisResult result)
        {

           
            var reply = context.MakeMessage();
            //get the reply from database

            reply.Text = "Hello, I am your friendly SharePoint Helper bot, ask my anything about SharePoint.";
            await context.PostAsync(reply);

            context.Wait(MessageReceived);
        }
        [LuisIntent("get general information")]
        private async Task GetGeneralInformation(IDialogContext context, LuisResult result)
        {

#region ExtractingEntites
            EntityRecommendation task;
            if (!result.TryFindEntity(Entity_Task, out task))
            {
                task = null;
            }
            EntityRecommendation subject;
            if (!result.TryFindEntity(Entity_Subject, out subject))     
            {
                subject = null;
            }
            EntityRecommendation platform;
            if (!result.TryFindEntity(Entity_Platform, out platform))
            {
                platform = null;
            }
            EntityRecommendation location;
            if (!result.TryFindEntity(Entity_Location, out location))
            {
                location = null;
            }
            EntityRecommendation cellphone;
            if (!result.TryFindEntity(Entity_CellPhone, out cellphone))
            {
                cellphone = null;
            }
            EntityRecommendation create;
            if (!result.TryFindEntity(Entity_Create, out create))
            {
                create = null;
            }
            #endregion
            var taskEntity = task != null ? task.Entity : null;
            var subEntity = subject != null ? subject.Entity : null;
            var platformEntity = platform != null ? platform.Entity : null;
            var locationEntity = location != null ? location.Entity : null;
            var cellphoneEntity = cellphone != null ? cellphone.Type : null;
            var createEntity = create != null ? create.Type : null;
            createEntity = taskEntity != null ? taskEntity : createEntity;
            //var dbQueryPattern = task.Entity + " " 
            var dbanswer = _allFAQs.Where(x => x.Task == createEntity)
                                 .Where(x => x.Subject == subEntity)
                                 .Where(x => x.Platform == platformEntity || x.Platform == cellphoneEntity)
                                 .Where(x => x.Location == locationEntity).FirstOrDefault();

            var answer = dbanswer != null ? dbanswer.Answer : "I dont find anything in the DB";
            var ansClassification  = dbanswer != null ? dbanswer.Classification : "NA";
            var reply = context.MakeMessage();
            //get the reply from database
            reply.Text = $"Here is your answer : {answer}";
            await context.PostAsync(reply);

            reply.Text = $"Classification : {ansClassification}";
            await context.PostAsync(reply);

            context.Wait(MessageReceived);
        }

        [LuisIntent("general request")]
        private async Task GeneralRequest(IDialogContext context, LuisResult result)
        {

            #region ExtractingEntites
            EntityRecommendation task;
            if (!result.TryFindEntity(Entity_Task, out task))
            {
                task = null;
            }
            EntityRecommendation subject;
            if (!result.TryFindEntity(Entity_Subject, out subject))
            {
                subject = null;
            }
            EntityRecommendation platform;
            if (!result.TryFindEntity(Entity_Platform, out platform))
            {
                platform = null;
            }
            EntityRecommendation location;
            if (!result.TryFindEntity(Entity_Location, out location))
            {
                location = null;
            }
            EntityRecommendation cellphone;
            if (!result.TryFindEntity(Entity_CellPhone, out cellphone))
            {
                cellphone = null;
            }
            EntityRecommendation create;
            if (!result.TryFindEntity(Entity_Create, out create))
            {
                create = null;
            }
            #endregion
            var taskEntity = task != null ? task.Entity : null;
            var subEntity = subject != null ? subject.Entity : null;
            var platformEntity = platform != null ? platform.Entity : null;
            var locationEntity = location != null ? location.Entity : null;
            var cellphoneEntity = cellphone != null ? cellphone.Type : null;
            var createEntity = create != null ? create.Type : null;
            createEntity = taskEntity != null ? taskEntity : createEntity;
            //var dbQueryPattern = task.Entity + " " 
            var dbanswer = _allFAQs.Where(x => x.Task == createEntity)
                                 .Where(x => x.Subject == subEntity)
                                 .Where(x => x.Platform == platformEntity || x.Platform == cellphoneEntity)
                                 .Where(x => x.Location == locationEntity).FirstOrDefault();

            var answer = dbanswer != null ? dbanswer.Answer : "I dont find anything in the DB";
            var ansClassification = dbanswer != null ? dbanswer.Classification : "NA";
            var reply = context.MakeMessage();
            //get the reply from database
            reply.Text = $"Here is your answer : {answer}";
            await context.PostAsync(reply);

            reply.Text = $"Classification : {ansClassification}";
            await context.PostAsync(reply);

            context.Wait(MessageReceived);
        }
    }
}