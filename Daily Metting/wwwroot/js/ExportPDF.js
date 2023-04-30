$(document).ready(function () {

//const date = new Date();

const monthNames = ["January", "February", "March", "April", "May", "June",
    "July", "August", "September", "October", "November", "December"
];


// Today's Pdf Date
var pdfdate = document.getElementById('pdfDate');
var datepdf = new Date();
let year = datepdf.getFullYear();
let month = monthNames[datepdf.getMonth()];
let day = datepdf.getDate();

let pdfMonth = (datepdf.getMonth() + 1).toString().padStart(2, '0');
//pdfdate.value = day + '/' + pdfMonth + '/' + year;


//get today's date
var currentDate = "" + month + " " + day + ", " + year;

// Event if i select a date 
	$('#date').change(function () {
	//get selected date
	var selectedDate = $('#date').val();
	console.log("selected date : " + selectedDate);
	var date = new Date(selectedDate);
	year = date.getFullYear();
	month = monthNames[date.getMonth()];
	day = date.getDate();

	//let pdfMonth = datepdf.getMonth() + 1;
	let pdfMonth = (date.getMonth() + 1).toString().padStart(2, '0');

	pdfdate.value = day + '/' + pdfMonth + '/' + year;

	console.log("pdf date:" + pdfdate.value);

	//get selected date 
		currentDate = "" + month + " " + day + ", " + year;
		console.log("current date : sel :"+currentDate);

	// Validate the selected date
	var today = new Date();
	var selected = new Date(selectedDate);
	


	if (selected > today) {
		alert('Please select a date that is not greater than today.');
		return;
	}
	$.ajax({
		url: '/Admin/Index',
		type: 'Get',
		data: { date: selectedDate },
		success: function (result) {
			$('#MyTable').html(result);
		},
		error: function () {
			alert('An error occurred while fetching the filtered data.');
		}
	});
});





let Title = "Daily meeting Report of " + currentDate;
//Console.log(currentDate);


$("#btnSubmit").click(function () {
        let Escalation = document.getElementById("escalation").value;
        let Notes = document.getElementById("escalation-comment").value;
	let MyView = `<!DOCTYPE html>
<html>
<head>
	<title>Meeting Report</title>
	<link rel="stylesheet" href="https://stackpath.bootstrapcdn.com/bootstrap/4.3.1/css/bootstrap.min.css" integrity="sha384-ggOyR0iXCbMQv3Xipma34MD+dH/1fQ784/j6cY/iJTQUOhcWr7x9JvoRxT2MZw1T" crossorigin="anonymous"/>
	<link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/4.7.0/css/font-awesome.min.css">
	<style>
		body {
		font-family: Arial, sans-serif;
			font-size: 14px;
			color: #333333;
			background-color: #ffffff;
			margin: 0;
			padding: 0;
		}

		section {
			background-color: #ffffff;
			/*padding: 20px;*/
			box-shadow: 0 0 10px rgba(0, 0, 0, 0.1);
			/*margin: 20px;*/
			border-radius: 5px;
		}
		h2 {
			font-size: 26px;
			margin-left: auto;
			margin-right: auto;
			text-shadow: 2px 2px 2px rgba(0, 0, 0, 0.1);
			color: #FF2E2E;
			text-align: center;
		}

		h4 {
			font-size: 16px;
			margin-top: 0;
			margin-bottom: 10px;
			color: #d41a1a;
		}

		p {
			line-height: 1.5;
			margin: 0;
			padding: 0;
		}

		table {
			border-radius: 5px;
            font-size:12px;
			position: relative;

		}
        th, td {
			padding: 10px;
			text-align: center;
			border-bottom: 1px solid #dddddd;
		}

		th {
			background-color: #d41a1a;
			color: #ffffff;
		}
		

	</style>
</head>
<body>
	<div>
		<nav class="navbar navbar-light bg-light">
			<div class="container">
				<a class="navbar-brand" href="#">
					<img style="display: inline-block; background-color: #ffffff;background-repeat: no-repeat;background-size: contain;margin-right: 20px;vertical-align: middle;height : 80px;" 
			src="/img/logo.png"
						 alt="MDB Logo"
						 loading="lazy" />
				</a>
			</div>
		</nav>
    <br/>

	<main> 
    <h2>Daily Meeting Report : <div class="date">`+ currentDate +`</div></h2>
		<section>
			<h4>Escalation :</h4>
			<p>`+ Escalation +`</p>
        </section>
        <br/>
        <section>
            <h4>Notes :</h4>
			<p>`+ Notes +`</p>
        </section>
        <br/>
        <section>

`
    MyView += $('#MyTable').html();
	MyView += "</section></main></div></body></html>";
	const rootUrl = window.location.origin;
	const ImgFolderUrl = rootUrl +"/img/Icons"
	

	let Result = MyView.replaceAll("/img/Icons", ImgFolderUrl);
	let View = Result.replaceAll(" Icon.svg", "%20Icon.svg");
	$("input[name='ExportData']").val(View);
	//$("input[name='pdfDate']").val(pdfdate.value);
});


});
