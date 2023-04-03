const date = new Date();

const monthNames = ["January", "February", "March", "April", "May", "June",
    "July", "August", "September", "October", "November", "December"
];

let day = date.getDate();
//let month = date.getMonth() + 1;
let month = monthNames[date.getMonth()];
let year = date.getFullYear();

// This arrangement can be altered based on how we want the date's format to appear.
let currentDate = ""+month+" "+day+", "+year;
let Title = "Daily meeting Report of " + currentDate;
console.log(currentDate);


$("#btnSubmit").click(function () {
        let Escalation = document.getElementById("escalation").value;
        let Notes = document.getElementById("escalation-comment").value;
	let MyView = `<!DOCTYPE html>
<html>
<head>
	<title>Meeting Report</title>
	<link rel="stylesheet" href="https://stackpath.bootstrapcdn.com/bootstrap/4.3.1/css/bootstrap.min.css" integrity="sha384-ggOyR0iXCbMQv3Xipma34MD+dH/1fQ784/j6cY/iJTQUOhcWr7x9JvoRxT2MZw1T" crossorigin="anonymous"/>
	<style>
		body {
		font-family: Arial, sans-serif;
			font-size: 14px;
			color: #333333;
			background-color: #ff6363;
			margin: 0;
			padding: 0;
		}

		section {
			background-color: #ffffff;
			padding: 20px;
			box-shadow: 0 0 10px rgba(0, 0, 0, 0.1);
			margin: 20px;
			border-radius: 5px;
		}
		h2 {
			font-size: 26px;
			margin-left: auto;
			margin-right: auto;
			text-shadow: 2px 2px 2px rgba(0, 0, 0, 0.1);
			color: #ffffff;
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
    <h2>Daily Meeting Report : <div class="date">March 29, 2023</div></h2>
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
	console.log(rootUrl);
	console.log(MyView);

	let Result = MyView.replaceAll("/img/Icons", ImgFolderUrl);
	let View = Result.replaceAll(" Icon.svg", "%20Icon.svg");
	console.log(View);
	$("input[name='ExportData']").val(View);
});
