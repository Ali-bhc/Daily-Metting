// get references to the timer and buttons
const timer = document.getElementById("timer");
const startBtn = document.getElementById("start-btn");
const pauseBtn = document.getElementById("pause-btn");
const resetBtn = document.getElementById("reset-btn");

// set the timer duration in seconds
const duration = 60*30;

// set a variable to store the timer interval ID
let timerInterval;

// set a variable to store the current time remaining
let timeRemaining = duration;

// function to format the time as mm:ss
function formatTime(time) {
    const minutes = Math.floor(time / 60);
    const seconds = time % 60;
    return `${minutes.toString().padStart(2, "0")}:${seconds.toString().padStart(2, "0")}`;
}

// function to update the timer display
function updateTimerDisplay() {
    timer.textContent = formatTime(timeRemaining);
}

// function to start the timer
function startTimer() {
    // clear any existing timer interval
    clearInterval(timerInterval);

    // set the timer interval to update the time remaining every second
    timerInterval = setInterval(() => {
        timeRemaining--;
        updateTimerDisplay();
    }, 1000);

    // disable the start button
    startBtn.disabled = true;
}

// function to pause the timer
function pauseTimer() {
    //Console.log("test");
    // clear the timer interval
    clearInterval(timerInterval);

    // enable the start button
    startBtn.disabled = false;

    // change the pause button text to "continue"
    pauseBtn.textContent = "Continue";
}

// function to continue the timer
function continueTimer() {
    // set the timer interval to update the time remaining every second
    timerInterval = setInterval(() => {
        timeRemaining--;
        updateTimerDisplay();
    }, 1000);

    // disable the start button
    startBtn.disabled = true;

    // change the pause button text back to "pause"
    pauseBtn.textContent = "Pause";
}

// function to reset the timer
function resetTimer() {
    // clear the timer interval
    clearInterval(timerInterval);

    // reset the time remaining
    timeRemaining = duration;

    // update the timer display
    updateTimerDisplay();

    // enable the start button
    startBtn.disabled = false;

    // change the pause button text back to "pause"
    pauseBtn.textContent = "Pause";
}

// add event listeners to the buttons
startBtn.addEventListener("click", startTimer);
pauseBtn.addEventListener("click", () => {
    if (pauseBtn.textContent === "Pause") {
        pauseTimer();
    } else {
        continueTimer();
    }
});
resetBtn.addEventListener("click", resetTimer);

// initialize the timer display
updateTimerDisplay();
