//Update a single vraagbaak
var updateVraagbaak = function (functie, naam, changer) {
    $('#' + functie).html(naam);
    //Also set the title, so people can see who updated it
    $('#' + functie).parent().attr("title", "Geplaatst door " + changer);
}

//Update all vraagbaken at once
var updateVraagbaken = function (vraagbaken) {
    for (var i = 0; i < vraagbaken.length; i++) {
        updateVraagbaak(vraagbaken[i]["Functie"], vraagbaken[i]["Naam"], vraagbaken[i]["ModifiedBy"]);
    }
}

var editVraagbaak = function (functie) {
    swal({
        title: functie + " van de dag wijzigen",
        text: "Geef de naam van de nieuwe " + functie + " van de dag op",
        type: "input",
        showCancelButton: true,
        closeOnConfirm: false,
        animation: "slide-from-top",
        inputPlaceholder: "Naam..."
    },
        function (inputValue) {
            if (inputValue === false) return false;

            if (inputValue === "") {
                swal.showInputError("Geef een naam op");
                return false
            }

            if (sendEditVraagbaak(functie, inputValue) != null) {
                swal({
                    title: "Done!",
                    text: "De nieuwe " + functie + " van de dag is " + inputValue,
                    type: "success",
                    showCancelButton: false,
                });
            } else {
                swal({
                    title: "Fout!",
                    text: "Het wijzigen van de vraagbaak is niet gelukt, probeer de pagina te verversen.",
                    type: "error",
                    showCancelButton: false,
                });
            }
        });
}