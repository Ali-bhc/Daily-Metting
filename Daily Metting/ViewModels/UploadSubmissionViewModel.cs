using System.ComponentModel.DataAnnotations;

namespace Daily_Metting.ViewModels
{
     public class UploadSubmissionViewModel
     {

        [Required(ErrorMessage = "Please select a file")]
        [DataType(DataType.Upload)]
        public IFormFile File { get; set; }

        public bool IsSubmitted { get; set; } = false;
        public string User { get; set; } = "";
        public string ValidationMessage { get; set; } = "";

        public UploadSubmissionViewModel(bool isSubmitted)
        {
            IsSubmitted = isSubmitted;
        }

        public UploadSubmissionViewModel()
        {
        }

        public UploadSubmissionViewModel(bool isSubmitted,string user)
        {
            IsSubmitted = isSubmitted;
            User = user;
        }

        public UploadSubmissionViewModel(bool isSubmitted, string user, string validationMessage) : this(isSubmitted, user)
        {
            ValidationMessage = validationMessage;
        }
    }

}
