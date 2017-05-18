function NewGame() {
    $.ajax({
        type: "GET",
        url: "/Home/NewGame",
        success: function (response) {
            window.location.href = "/Home/Game";
        }
    });
}

function EditConfiguration() {
    $('#GameSettingsWindow').modal('show');
    autoFocus('#GameSettingsWindow', '#queens_number');
    $.ajax({
        type: "GET",
        url: "/Home/GetConfiguration",
        dataType: "json",

        success: function (response) {
            $("#queens_number").val(response.QueensNumber);
            $("#workers_number").val(response.WorkersNumber);
            $("#drones_number").val(response.DronesNumber);
            $("#queens_lifespan").val(response.QueensLifespan);
            $("#queens_hitpoints").val(response.QueensHitpoints);
            $("#workers_lifespan").val(response.WorkersLifespan);
            $("#workers_hitpoints").val(response.WorkersHitpoints);
            $("#drones_lifespan").val(response.DronesLifespan);
            $("#drones_hitpoints").val(response.DronesHitpoints);
        }
    });
}

function SaveSettings(savingtype) {
    var gameConfiguration = {
        QueensNumber: $("#queens_number").val(),
        WorkersNumber: $("#workers_number").val(),
        DronesNumber: $("#drones_number").val(),
        QueensLifespan: $("#queens_lifespan").val(),
        QueensHitpoints: $("#queens_hitpoints").val(),
        WorkersLifespan: $("#workers_lifespan").val(),
        WorkersHitpoints: $("#workers_hitpoints").val(),
        DronesLifespan: $("#drones_lifespan").val(),
        DronesHitpoints: $("#drones_hitpoints").val()
    };

    $.ajax({
        type: "POST",
        data: gameConfiguration,
        url: "/Home/SaveConfiguration",

        success: function (response) {
            $('#GameSettingsWindow').modal('toggle');
            if (savingtype == "save&start")
                NewGame();
        }
    });
}

function HitBee(beesNumber) {
    var id="random";

    for (var i = 0; i < beesNumber; i++) {
        if(document.getElementById(i+"bee").getAttribute("isselected")=="yes"){
            id=i;
            document.getElementById(i+"bee").setAttribute("isselected", "no");
            break;
        }
    }

    $.ajax({
        type: "GET",
        url: "/Home/HitBee?id=" + id,
        success: function (response) {
            if (response == "Victory") {
                for (var i = 0; i < beesNumber; i++) {
                    document.getElementById(i+"bee").innerHTML = "Dead";
                }
                $('#VictoryWindow').modal('show');
            }
            else {
                var responses = response.split("LP");

                if (responses[1] <= 0)
                    document.getElementById(responses[0]+"bee").innerHTML = "Dead";
                else
                    document.getElementById(responses[0]+"bee").innerHTML = responses[1];
            }
        }
    });
}

function SelectBee(id, beesNumber) {
    if(document.getElementById(id).innerHTML=="Dead")
    {
        alert("Already dead");
        return;
    }
    for (var i = 0; i < beesNumber; i++) {
        if(document.getElementById(i+"bee").getAttribute("isselected")=="yes"){
            document.getElementById(i+"bee").setAttribute("isselected", "no");
            break;
        }
    }
    document.getElementById(id).setAttribute("isselected", "yes");
}

function SaveGame(savingtype){
    $.ajax({
        type: "GET",
        url: "/Home/SaveGame?savingtype=" + savingtype,
        success: function (response) {
            $('#SaveGameWindow').modal('toggle');
            alert("Saved.");
        }
    });
}

function LoadGame(loadingtype) {
    $.ajax({
        type: "GET",
        url: "/Home/LoadGame?loadingtype=" + loadingtype,
        success: function (response) {
            if (response != "Save is not found.")
                window.location.href = "/Home/Game";
            alert(response);
        }
    });
}

function ConfirmRestart() {
    if (confirm("Are you sure?") == true) {
        NewGame();
    }
}

function autoFocus(modal, field) {
    $(modal).on('shown.bs.modal', function () {
        $(field).focus();
    });
}