using System.ComponentModel.DataAnnotations;

namespace wsIncidents.Models {

    public class authModel {

        /// <summary>
        /// User Name
        /// </summary>
        [DataType(DataType.Text)]
        [Required(ErrorMessage = "{0} is required")]
        [StringLength(50, MinimumLength = 1, ErrorMessage = "{0} should be minimun 1 characters, and a maximun of 50 characters")]
        public string user { get; set; }

        /// <summary>
        /// Password
        /// </summary>
        [DataType(DataType.Text)]
        [Required(ErrorMessage = "{0} is required")]
        [StringLength(50,MinimumLength = 1,ErrorMessage = "{0} should be minimun 1 characters, and a maximun of 50 characters")]
        public string password { get; set; }

        public int codusr { get; set; }
    }
}
