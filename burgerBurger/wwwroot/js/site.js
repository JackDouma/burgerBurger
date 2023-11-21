// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// function to show the scheduling input field or not
function showSchedule() {
    var cb = document.getElementById('scheduleCheck').checked;
    var form = document.getElementById('delivery');

    if (cb)
        form.className = 'form-group';
    else
        form.className = 'form-group d-none';
}

// function to add the value of the clicked ingredient checkbox
// to the input field that can be read by the controller
function populateIngredients() {
    var checkboxes = document.getElementById("ingredients");
    var ingredients = String();
    for (const child of checkboxes.children) {
        if (child.checked) {
            ingredients += (child.value + " ");
        }
    }
    document.getElementById("ingredientsInput").value = ingredients;
}

function showMeat() {
    document.getElementById('meat').className = "";
}

function showOtherIngredients() {
    document.getElementById('toppings').className = "";
    document.getElementById('condiments').className = "";
}

function populateIngredientsCustom() {
    var checkboxes = document.getElementById("ingredients");
    var ingredients = String();
    for (const child of checkboxes.children) {
        for (const secondChild of child.children) {
            if (secondChild.checked) {
                ingredients += (secondChild.value + " ");
            }
        }
    }
    document.getElementById("ingredientsInput").value = ingredients;
}
