using System;

namespace TabloidMVC.Models
{
    public class Sub
    {
        public int Id { get; set; }
        public int SubscriberUserProfileId { get; set; }
        public UserProfile Subscriber { get; set; }
        public int ProviderUserProfileId { get; set; }
        public UserProfile Provider { get; set; }
        public DateTime BeginDateTime { get; set; }
        public DateTime ? EndDateTime { get; set; }
    }
}
