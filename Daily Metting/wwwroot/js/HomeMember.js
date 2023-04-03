$('#call-to-action,.click-upload').click(function () {
    $('#call-to-action').addClass('upload--loading');
    $('.upload-hidden').click();
});

//document.getElementById("UploadBtn").addEventListener("click", validateForm);
//function validateForm() {
$("#UploadBtn").click(function () {
    var user = this.getAttribute("data-value");
    console.log(user+"    User");

    var fileInput = document.getElementById('file-input');
    console.log(fileInput.innerText)

    if (fileInput.value == '') {
        alert('Please select an Excel a file to upload.');
        return false;
    }
    else {
        const fileName = fileInput.value.split('\\').pop();
        console.log(fileName);
        //let isvalid = FileNameValidation();

        //if (fileName.includes(user)) {
        //    return true;
        //}
        //else
        //    return false;
        let IsValid = false;
        switch (user) {
            case "WH":
                console.log(user + "WH");
                IsValid = fileName.includes("DailyMeetingWareHouseFile");
                break;
            case "CS_PP":
                IsValid = fileName.includes("DailyMeetingCS_PP_File");
            case "Procurement":
                IsValid = fileName.includes("DailyMeetingProcurementFile");
                break;
        }
        console.log(fileName.includes("DailyMeetingWareHouseFile"));
        if (IsValid) {
            return true;
        }
        else {
            alert('Please select the correct Excel file or check the name of file!');
            return false;
        }
    }

    });

