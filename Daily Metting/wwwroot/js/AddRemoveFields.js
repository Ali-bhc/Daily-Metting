// Counter for unique IDs of input fields
let counter = 1;

// Add Field button click event
$("#add-field").click(function () {
    // Create a new input field group
    let newFieldGroup = `
                    <label for="value-${counter}">Value:</label>
                    <input type="text" class="form-control" id="value-${counter}" name="value-${counter}">
                    <label for="description-${counter}">Description:</label>
                    <input type="text" class="form-control" id="description-${counter}" name="description-${counter}">
                    <label for="comments-${counter}">Comments:</label>
                    <input type="text" class="form-control" id="comments-${counter}" name="comments-${counter}">
            `;
    // Add the new field group to the fields container
    $("#new-fields").append(newFieldGroup);
    // Increment the counter for the next input field group
    counter++;
    // Show the Remove Field button
    $("#remove-field").show();
    console.log("insertiooon heerer")
});

// Remove Field button click event
$("#remove-field").click(function () {
    // Remove the last input field group
    $("#fields-container .form-group:last-child").remove();
    // Decrement the counter
    counter--;
    // Hide the Remove Field button if there are no more input field groups
    if (counter == 1) {
        $("#remove-field").hide();
    }
});