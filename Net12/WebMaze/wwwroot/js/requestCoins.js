$(document).ready
   function validateForm() {
    let x = document.forms["myForm"]["userName"].value;
    if (x == "") {
        alert("Fild must be filled");
        return false;
    }
}