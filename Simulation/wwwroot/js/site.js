function ResetInput(object) {
    object.classList.remove('is-invalid');
    object.classList.remove('is-valid');
}
function SetInputState(object,state) {
    if(state == 'invalid') {
        ResetInput(object);
        object.classList.add('is-invalid');
    }
    else if(state == 'valid') {
        ResetInput(object);
        object.classList.add('is-valid');
    }
    else {
        ResetInput(object);
    }
}
function CheckPassword(object) {
    if(object.value.length < 8) {
        SetInputState(object, 'invalid');
    }
    else {
        SetInputState(object, 'valid');
    }
}
function CheckConfirm(object,objectId) {
    var secondObject = document.getElementById(objectId);
    if(object.value == secondObject.value) {
        SetInputState(object, 'valid');
    }
    else {
        SetInputState(object, 'invalid');
    }
}