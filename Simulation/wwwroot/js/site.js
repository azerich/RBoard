function ClearIconClassList(object) {
    object.classList.remove('text-danger');
    object.classList.remove('text-success');
    object.classList.remove('text-warning');
}
function SetIconClassList(object, state) {
    ClearIconClassList(object);
    if (state == 'set danger') {
        object.classList.add('text-danger');
    }
    else if (state == 'set success') {
        object.classList.add('text-success');
    }
    else {
        //do nothing
    }
}
function ClearInputClassList(object) {
    object.classList.remove('is-invalid');
    object.classList.remove('is-valid');
}
function SetInputClassList(object, state) {
    ClearInputClassList(object);
    if (state == 'set invalid') {
        object.classList.add('is-invalid');
    }
    else if (state == 'set valid') {
        object.classList.add('is-valid');
    }
    else {
        //do nothing
    }
}
function CheckUserName(object) {
    var icon = document.getElementById('UserNameSign');
    if (object.value.length < 5) {
        SetInputClassList(object, 'set invalid');
        SetIconClassList(icon, 'set danger');
    }
    else {
        var link = '/Account/CheckUserName/' + object.value;
        $.ajax(link,{
            method: 'GET',
            success: function (response) {
                console.alert(response);
            },
            error: function () {
                SetIconClassList(icon, 'set danger');
                SetInputClassList(object, 'set invalid');
                console.alert('Could not connect to server');
            }
        });
    }
}
function CheckPassword(object) {
    var icon = document.getElementById('PasswordSign');
    var errorMessage = "";
    var allLetters = /^[a-zA-Z]+$/;
    var letter = /[a-zA-Z]/;
    var number = /[0-9]/;

    if (!object.value.length < 8) {
        SetInputClassList(object, 'set invalid');
        SetIconClassList(icon, 'set danger');
        errorMessage += "Password must be longer than 7 characters.\n";
    };
    if (!allLetters.test(object)) {
        SetInputClassList(object, 'set invalid');
        SetIconClassList(icon, 'set danger');
        errorMessage += "Password must contain mass letters.\n";
    };
    if (!letter.test(object)) {
        SetInputClassList(object, 'set invalid');
        SetIconClassList(icon, 'set danger');
        errorMessage += "Password must contain letters.\n";
    }
    if (!number.test(object)) {
        SetInputClassList(object, 'set invalid');
        SetIconClassList(icon, 'set danger');
        errorMessage += "Password must contain digits.\n";
    }
    console.a
}