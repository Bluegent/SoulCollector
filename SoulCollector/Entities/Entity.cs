using System.Collections.Generic;

namespace SoulCollector.Entities
{
    public class BaseEntity
    {
        protected Dictionary<StatType, Stat> Stats;
        protected Dictionary<ResourceType, Resource> Resources;

        public BaseEntity()
        {
            Stats= new Dictionary<StatType, Stat>();
            Resources = new Dictionary<ResourceType, Resource>();
            Resource health = new Resource(ResourceType.Health, 100, 100);
            Resources.Add(health.Type,health);
        }

    }
}