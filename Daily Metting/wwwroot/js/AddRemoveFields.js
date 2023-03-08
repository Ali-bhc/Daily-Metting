// Counter for unique IDs of input fields
let counter = 1;
let pn = counter+1;
let PnName = "Part Number" + pn;

// Add Field button click event
$("#add-field").click(function () {

    
    // get counter value 
    var intValue = parseInt(this.getAttribute("data-value1"));
    var newValue = + intValue;
    var pointID = parseInt(this.getAttribute("data-value2"));

    var dataValue = document.getElementById("add-field");
    dataValue.setAttribute("data-value1", newValue);
    
    // Create a new input field group
    let newFieldGroup =
        `<div class="form-group">
                            <label for="safety-pt1">test</label>
                            <input name="Values[`+ intValue +`].Value_point" type="number" class="form-control" id="safety-pt1">
                            <textarea name="Values[`+ intValue +`].description class="form-control" id="safety-pt1-desc" placeholder="Description"></textarea>
                            <textarea name="Values[`+ intValue + `].comment class="form-control" id="safety-pt1-comment" placeholder="Comment"></textarea>
                            <input name="Values[`+ intValue + `].PointID type="hidden" value=` + pointID +` class="form-control" id="safety-pt1">
                            <input name="total_number" type="hidden" value=` + newValue +` class="form-control" id="safety-pt1">
                        </div>`;
    // Add the new field group to the fields container
    $("#new-fields").append(newFieldGroup);
    console.log(counter)
    // Increment the counter for the next input field group
    counter++;
    // Show the Remove Field button
    $("#remove-field").show();
});

// Remove Field button click event
$("#remove-field").click(function () {
    // Remove the last input field group
    $("#new-fields .form-group:last-child").remove();
    // Decrement the counter
    counter--;
    // Hide the Remove Field button if there are no more input field groups
    if (counter == 1) {
        $("#remove-field").hide();
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

