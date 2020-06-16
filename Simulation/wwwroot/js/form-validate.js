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
passwordInput.addEventListener('keypress', CheckPassword);
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
            if(String(message).length != 0) {
                feedback.classList.add('valid-feedback');
                feedback.innerHTML = String(message);
            }
            break;
        case 'invalid':
            group.classList.add('has-danger');
            icon.classList.add('text-danger');
            input.classList.add('is-invalid');
            if(String(message).length != 0) { 
                feedback.classList.add('invalid-feedback');
                feedback.innerHTML = message;
            }
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
function CheckEmail() {
    let result = true;
    let ul = document.createElement('ul');
    if(!CheckEmailFormat(emailInput)) {
        let li = document.createElement('li');
        li.innerHTML = emailFormatError;
        ul.appendChild(li);
        result = false;
    } 
    if(!CheckEmailAvailability(emailInput)) {
        let li = document.createElement('li');
        li.innerHTML = emailAvailabilityError;
        ul.appendChild(li);
        result = false;
    }    
    if(!result) SetState('email', 'invalid', ul.innerHTML);
    else SetState('email', 'valid', successMessage);
    return result;
}
function CheckPasswordLength(object) {
    if(object.value.length < passwordMinLength) return false;
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
function CheckPassword() {
    let ul = document.createElement('ul');
    let result = true;
    if(!CheckPasswordLength(passwordInput)) {
        let li = document.createElement('li');
        li.innerHTML = passwordLengthError;
        ul.appendChild(li);
        result = false;
    }
    if(!CheckPasswordDigitContain(passwordInput)) {
        let li = document.createElement('li');
        li.innerHTML = passwordContainDigitError;
        ul.appendChild(li);
        result = false;
    }
    if(!CheckPasswordUpperContain(passwordInput)) {
        let li = document.createElement('li');
        li.innerHTML = passwordContainUpperError;
        ul.appendChild(li);
        result = false;
    }
    if(!CheckPasswordSpecialContain(passwordInput)) {
        let li = document.createElement('li');
        li.innerHTML = passwordContainSpecialError;
        ul.appendChild(li);
        result = false;
    }

    if(!result) SetState('password', 'invalid', ul.innerHTML);
    else SetState('password', 'valid', successMessage);

    return result;
}
function CheckConfirm() {
    let result = true;
    if(String(confirmInput.value).length == 0 || String(confirmInput.value) != String(passwordInput.value)) {
        SetState('confirm', 'invalid', confirmError);
        result = false;
    }
    else {
        SetState('confirm', 'valid', successMessage);        
        result = true;
    }
    return result;
}
function ValidateRegistration() {
    return CheckEmail() && CheckPassword() && CheckConfirm();
}
function ValidatePasswordAndConfirm() {
    return CheckPassword() && CheckConfirm();
}