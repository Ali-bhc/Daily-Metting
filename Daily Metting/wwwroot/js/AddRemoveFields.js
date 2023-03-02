// Counter for unique IDs of input fields
let counter = 1;
let pn = counter+1;
let PnName ="Part Number"+ pn ;

// Add Field button click event
$("#add-field").click(function () {
    // Create a new input field group
    let newFieldGroup = 
                   '<div class="form-group">'+
                   '<label for="safety-pt1">Add AN OTHER PN</label>'+
                   '<input asp-for="Values[counter].Value_point" type="text" class="form-control" id="safety-pt1" name="safety-pt1">'+
                   '<textarea asp-for="Values[counter].description" class="form-control" id="safety-pt1-desc" name="safety-pt1-desc" placeholder="Description"></textarea>'+
                   '<textarea asp-for="Values[counter].comment" class="form-control" id="safety-pt1-comment" name="safety-pt1-comment" placeholder="Comment"></textarea>'+
                   '</div>';
    // Add the new field group to the fields container
    $("#new-fields").append(newFieldGroup);
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
});