$(document).ready(function () {

    const Safety = document.getElementById("Safety");
    if (Safety != null) { Safety.style.backgroundColor = "#F1C40F"; }

    const People = document.getElementById("People");
    if (People != null) { People.style.backgroundColor = "#0074D9"; }

    const Quality = document.getElementById("Quality");
    if (Quality != null) { Quality.style.backgroundColor = "#2ca9bc"; }

    const Delivery = document.getElementById("Delivery");
    if (Delivery != null) { Delivery.style.backgroundColor = "#FFA500"; }

    const Cost = document.getElementById("Cost");
    if (Cost != null) { Cost.style.backgroundColor = "#008000"; }


    // Set the value of the hidden input to today's date
    var today = new Date();
    var year = today.getFullYear();
    var month = ('0' + (today.getMonth() + 1)).slice(-2);
    var day = ('0' + today.getDate()).slice(-2);
    //document.getElementById('pdfDate').value = day + '/' + month + '/' + year;


    //$('#date').change(function () {
    //    //var selectedDate = $('#date').val();

    //    //// Validate the selected date
    //    //var today = new Date();
    //    //var selected = new Date(selectedDate);
    //    //if (selected > today) {
    //    //    alert('Please select a date that is not greater than today.');
    //    //    return;
    //    //}
    //    ////Console.log(selected);
    //    //$.ajax({
    //    //    url: '/Admin/Index',
    //    //    type: 'Get',
    //    //    data: { date: selectedDate },
    //    //    success: function (result) {
    //    //        $('#MyTable').html(result);
    //    //    },
    //    //    error: function () {
    //    //        alert('An error occurred while fetching the filtered data.');
    //    //    }
    //    //});
    //  //  document.getElementById('pdfDate').value = selectedDate;
    //});

    const table = document.getElementById('AttainementTable');
    if (table != null) {
    const rows = table.rows;
        for (let i = 2; i < rows.length; i++) {
            const cells = rows[i].cells;
            for (let j = 1; j < cells.length - 1; j++) {
                const cell = cells[j];
                let result = (cell.innerText).slice(0, -1);
                const value = parseInt(result);

                if (value > 100) {
                    cell.style.backgroundColor = '#9fff80';
                }
                else {
                    if (value > 90) {
                        cell.style.backgroundColor = '#ffff99';
                    }
                    else {
                        cell.style.backgroundColor = '#ff6666';
                    }
                }
            }
        }
    }

})
