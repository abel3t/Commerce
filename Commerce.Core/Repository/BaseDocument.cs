using MongoDB.Bson;

namespace Commerce.Core.Repository
{
    public class BaseDocument
    {
        private string _id;

        protected BaseDocument() => _id = ObjectId.GenerateNewId().ToString();

        public string Id
        {
            get { return _id; }
            set
            {
                _id = (string.IsNullOrEmpty(value)) ? ObjectId.GenerateNewId().ToString() : value;
            }
        }
    }
}