using System.Runtime.Serialization;

namespace GroupMeetingASP.NETCoreWebApp.Models
{
    [DataContract]
    public class DataPoint
    {
        //DataContract for Serializing Data - required to serve in JSON format
        
        
            public DataPoint(string label, double y)
            {
                this.Label = label;
                this.Y = y;
            }

            //Explicitly setting the name to be used while serializing to JSON.
            [DataMember(Name = "label")]
            public string Label = "";

            //Explicitly setting the name to be used while serializing to JSON.
            [DataMember(Name = "y")]
            public Nullable<double> Y = null;
        
    }
}
