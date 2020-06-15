//email
var emailGroup = document.getElementById('emailGroup');
var emailInput = document.getElementById('emailInput');
var emailIcon = document.getElementById('emailIcon');
var emailFeedback = document.getElementById('emailFeedback');
//password
var passwordGroup = document.getElementById('passwordGroup');
var passwordInput = document.getElementById('passwordInput');
var passwordIcon = document.getElementById('passwordIcon');
var passwordFeedback = document.getElementById('passwordFeedback');
//confirmPassword
var confirmGroup = document.getElementById('confirmGroup');
var confirmInput = document.getElementById('confirmInput');
var confirmIcon = document.getElementById('confirmIcon');
var confirmFeedback = document.getElementById('confirmFeedback');

function ClearGroupClassList(object) {
    object.classList.remove('has-danger');
    object.classList.remove('has-success');
}
function ClearIconClassList(object) {
    object.classList.remove('text-danger');
    object.classList.remove('text-success');
}
function ClearInputClassList(object) {
    object.classList.remove('is-invalid');
    object.classList.remove('is-valid');
}
function ClearFeedbackClassList(object) {
    object.classList.remove('invalid-feedback');
    object.classList.remove('valid-feedback');
    object.innerHTML = '';
}
function SetState(groupid, state, message) {
    switch(groupid) {
        case 'email':
            var group = emailGroup;
            var icon = emailIcon;
            var input = emailInput;
            var feedback = emailFeedback;
            break;
        case 'password':
            var group = passwordGroup;
            var icon = passwordIcon;
            var input = passwordInput;
            var feedback = passwordFeedback;
            break;
        case 'confirm':
            var group = confirmGroup;
            var icon = confirmIcon;
            var input = confirmInput;
            var feedback = confirmFeedback;
            break;
    }

    ClearGroupClassList(group);
    ClearIconClassList(icon);
    ClearInputClassList(input);
    ClearFeedbackClassList(feedback);
    
    switch(state){
        case 'valid':
            group.classList.add('has-success');
            icon.classList.add('text-success');
            input.classList.add('is-valid');
            feedback.classList.add('valid-feedback');
            feedback.innerHTML = message;
            break;
        case 'invalid':
            group.classList.add('has-danger');
            icon.classList.add('text-danger');
            input.classList.add('is-invalid');
            feedback.classList.add('invalid-feedback');
            feedback.innerHTML = message;
            break;
    }
}
function CheckEmailFormat(object) {
    const regexp = /^(([^<>()\[\]\\.,;:\s@"]+(\.[^<>()\[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
    return regexp.test(String(object.value).toLowerCase());
}
function CheckEmailAvailability(object) {
    // ajax call
    return true;
}
function CheckPasswordLength(object) {
    if(object.value.length < passwordMinLength || object.value.length > passwordMaxLength) return false;
    else return true;
}
function CheckPasswordDigitContain(object) {
    let regexp = /[\d]/;
    if(passwordContainDigit) return regexp.test(String(object.value));
    else return true;
}
function CheckPasswordUpperContain(object) {
    let regexp = /[A-Z]/;
    if(passwordContainUpper) return regexp.test(String(object.value));
    else return true;
}
function CheckPasswordSpecialContain(object) {
    let regexp = /[!@#$%^&*()_+-=?\/\\.,\'\";:{}\[\]]/;
    if(passwordContainSpecial) return regexp.test(String(object.value));
    else return true;
}
function Validate() {
    //email check
    if(!CheckEmailFormat(emailInput)) SetState('email', 'invalid', 'Please enter correct email');
    else if(!CheckEmailAvailability(emailInput)) SetState('email', 'invalid', 'This email is already taken. Try another');
    else if(CheckEmailFormat(emailInput) && CheckEmailAvailability(emailInput)) SetState('email', 'valid', 'Success');
    //password check
    if(!CheckPasswordLength(passwordInput)) SetState('password', 'invalid', 'Password must be between 8 and 16 characters');
    else if(!CheckPasswordDigitContain(passwordInput)) SetState('password', 'invalid', 'Password should contain at least one digit');
    else if(!CheckPasswordUpperContain(passwordInput)) SetState('password', 'invalid', 'Password should contain at least one character in upper case');
    else if(!CheckPasswordSpecialContain(passwordInput)) SetState('password', 'invalid', 'Password should contain at least one special character');
    else SetState('password', 'valid', 'Success');
    //confirm check
    if(String(confirmInput.value).length == 0 || String(passwordInput.value) != String(confirmInput.value)) SetState('confirm', 'invalid' , 'Password and confrim password must be match');
    else SetState('confirm', 'valid', 'Success');
    return false;
}