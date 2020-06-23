function ChangeInput(object) {
    let InputName = object.id.substring(4, object.id.length);
    let confirmIconObj = document.getElementById("confirm" + InputName);
    let cancelIconObj = document.getElementById("cancel" + InputName);
    let InputObj = document.getElementById(InputName);

    InputObj.classList.remove("form-control-plaintext");
    InputObj.classList.add("form-control");
    InputObj.readOnly = false;

    object.classList.add("fade");

    confirmIconObj.classList.remove("fade");
    cancelIconObj.classList.remove("fade");
}
function ConfirmInput(previousValue, object) {
    let InputName = object.id.substring(7, String(object.id).length);
    let InputObj = document.getElementById(InputName);
    let cancelIconObj = document.getElementById("cancel" + InputName);
    let editIconObj = document.getElementById("edit" + InputName);

    InputObj.readOnly = true;
    InputObj.classList.remove("form-control");
    InputObj.classList.add("form-control-plaintext");

    if (String(previousValue) !== String(InputObj.value)) {
        InputObj.classList.add("border-warning");
    }

    object.classList.add("fade");
    cancelIconObj.classList.add("fade");
    editIconObj.classList.remove("fade");
}
function CancelInput(previousValue, object) {
    let InputName = object.id.substring(6, String(object.id).length);
    let InputObj = document.getElementById(InputName);
    let confirmIconObj = document.getElementById("confirm" + InputName);
    let editIconObj = document.getElementById("edit" + InputName);

    InputObj.value = previousValue;
    InputObj.readOnly = true;
    InputObj.classList.remove("form-control");
    InputObj.classList.add("form-control-plaintext");
    InputObj.classList.remove("border-warning");

    object.classList.add("fade");
    confirmIconObj.classList.add("fade");
    editIconObj.classList.remove("fade");
}
function ChangeSelect(object) {
    let selectName = object.id.substring(4, object.id.length);
    let confirmIconObj = document.getElementById("confirm" + selectName);
    let cancelIconObj = document.getElementById("cancel" + selectName);
    let selectObj = document.getElementById(selectName);

    selectObj.disabled = false;
    selectObj.classList.remove("form-control-plaintext");
    selectObj.classList.add("form-control");

    object.classList.add("fade");

    confirmIconObj.classList.remove("fade");
    cancelIconObj.classList.remove("fade");
}
function ConfirmSelect(previousValue, object) {
    let selectName = object.id.substring(7, object.id.length);
    let editIconObj = document.getElementById("edit" + selectName);
    let cancelIconObj = document.getElementById("cancel" + selectName);
    let selectObj = document.getElementById(selectName);

    if (selectObj[selectObj.selectedIndex].value !== previousValue) {
        selectObj.classList.add("border-warning");
    }

    selectObj.disabled - true;

    object.classList.add("fade");
    cancelIconObj.classList.add("fade");

    editIconObj.classList.remove("fade");
}
function CancelSelect(previousValue, object) {
    let selectName = object.id.substring(6, object.id.length);
    let editIconObj = document.getElementById("edit" + selectName);
    let confirmIconObj = document.getElementById("confirm" + selectName);
    let selectObj = document.getElementById(selectName);

    selectObj[selectObj.selectedIndex].value = previousValue;

    selectObj.disabled = true;
    selectObj.classList.remove("border-warning");
    selectObj.classList.remove("form-control");
    selectObj.classList.add("form-control-plaintext");

    editIconObj.classList.remove("fade");

    object.classList.add("fade");
    confirmIconObj.classList.add("fade");
}