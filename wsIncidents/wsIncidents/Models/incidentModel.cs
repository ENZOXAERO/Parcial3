using System.Collections.Generic;

namespace wsIncidents.Models {

    public class incidentModel {

        public int code { get; set; }

        public string description { get; set; }

        public int type { get; set; }

        public string typeDescription { get; set; }

        public byte state { get; set; }

        public List<incidentDetailsModel> details { get; set; }

        public int? codusr { get; set; }

    }
}
