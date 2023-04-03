let counter = 0;
//let PnName = "Part Number" + pn;

// Add Field button click event
//const testing = document.querySelectorAll('.testID');
//testing.forEach(button => {
//    button.addEventListener('click', () => {
//        // get cnt value
//        var intValue = parseInt(button.getAttribute("data-value1"));
//        var pointID = parseInt(button.getAttribute("data-value2"));
//        let newFieldGroup =
//            `<div class="form-group">
//            <label for="safety-pt1">test</label>
//            <input name="Values[`+ intValue + `].Value_point" type="number" class="form-control" id="safety-pt1">
//            <textarea name="Values[`+ intValue + `].description" class="form-control" id="safety-pt1-desc" placeholder="Description"></textarea>
//            <textarea name="Values[`+ intValue + `].comment" class="form-control" id="safety-pt1-comment" placeholder="Comment"></textarea>
//            <input name="Values[`+ intValue + `].PointID" type="hidden" value=` + pointID + ` class="form-control" id="safety-pt1">
//        </div>`;
//        // Add the new field group to the fields container
//        $("#new-fields").append(newFieldGroup);
//        button.attr("data-value1", ++intValue);
//        console.log(cnt)
//        // Increment the cnt for the next input field group
//        cnt++;
//        console.log('Button clicked!');
//    });
//});


$(".add-field1").click(function () {

    //let pn = cnt + 1;
    // get cnt value 

    var pointName = this.getAttribute("data-value");
    console.log(pointName);
    var intValue = parseInt(this.getAttribute("data-value1"));
    var pointID = parseInt(this.getAttribute("data-value2"));

    counter = intValue;
    //// Create a new input field group
    //for (let i = 0; i < intValue ; i++)
    //{
    //    console.log("heeere");
    //let newFieldGroup =
    //    `<div class="form-group">
    //                        <label for="safety-pt1">test</label>
    //                        <input name="Values[`+ intValue + `].Value_point" type="number" class="form-control" id="safety-pt1">
    //                        <textarea name="Values[`+ intValue + `].description class="form-control" id="safety-pt1-desc" placeholder="Description"></textarea>
    //                        <textarea name="Values[`+ intValue + `].comment class="form-control" id="safety-pt1-comment" placeholder="Comment"></textarea>
    //                        <input name="Values[`+ intValue + `].PointID type="hidden" value=` + pointID + ` class="form-control" id="safety-pt1">
    //                        <input name="total_number" type="hidden" value=` + newValue + ` class="form-control" id="safety-pt1">
    //                    </div>`;
    //    $("#new-fields").append(newFieldGroup);


//}

    //// Create a new input field group
    let newFieldGroup =
        `<div class="form-group">
            <label for="safety-pt1">test</label>
            <input name="Values[`+ pointName + `][` + intValue +`].Value_point" type="number" class="form-control" id="safety-pt1">
            <textarea name="Values[`+ pointName + `][` + intValue +`].description" class="form-control" id="safety-pt1-desc" placeholder="Description"></textarea>
            <textarea name="Values[`+ pointName + `][` + intValue +`].comment" class="form-control" id="safety-pt1-comment" placeholder="Comment"></textarea>
            <input name="Values[`+ pointName + `][` + intValue +`].PointID" type="hidden" value=` + pointID +` class="form-control" id="safety-pt1">
        </div>`;
    // Add the new field group to the fields container
    var newFields = '#'+pointID+'-new-fields';
    console.log(newFields);
    $(newFields).append(newFieldGroup);
    this.setAttribute("data-value1", ++intValue);
    //console.log(cnt);
    // Increment the cnt for the next input field group
    counter++;
    // Show the Remove Field button
    $(".remove-field1").show();
});









// Remove Field button click event
$(".remove-field1").click(function () {

    var pointID = parseInt(this.getAttribute("data-value"));
    var newFields = '#' + pointID + '-new-fields';
    console.log(newFields);

    // Remove the last input field group
    $(newFields+" .form-group:last-child").remove();
    // Decrement the cnt
    counter--;
    // Hide the Remove Field button if there are no more input field groups
    if (counter == 1) {
        $(".remove-field").hide();
    }
})



function incrementDataValue(dataValue) {
    dataValue++;
    console.log("Incremented data value: " + dataValue);

    // Send new value back to server using AJAX
    $.ajax({
        url: "/updateDataValue",
        type: "POST",
        data: { newValue: dataValue },
        success: function (response) {
            console.log("Updated value on server");
            $("#dataValue").text(response.newValue);
        },
        error: function () {
            console.log("Failed to update value on server");
        }
    });
}



//$("body").on("load", () => {
//    //get data by ajax 
//    var data = {
//        pointCateg: [
//            {}
//            ]
//    }
//})
