using Domain.Domain.JournalItems;
using Domain.RequestModels.Journal.JournalItems;
using Newtonsoft.Json.Linq;
using System;

namespace Domain.JsonConverter
{
    public class JournalItemModelConverter : JsonCreationConverter<JournalItemModel>
    {
        protected override JournalItemModel Create(Type objectType, JObject jObject)
        {
            if (jObject == null) throw new ArgumentNullException("jObject");

            var journalItemType = jObject.Value<JournalItemType>("Type");

            if (journalItemType != JournalItemType.Folder)
            {
                return new JournalFolderModel();
            }
            else if (journalItemType != JournalItemType.Handout)
            {
                return new JournalHandoutModel();
            }
            else
            {
                return new JournalItemModel();
            }
        }
    }
}
