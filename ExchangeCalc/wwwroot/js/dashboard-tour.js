//document.addEventListener("DOMContentLoaded", () => {
//    const startBtn = document.getElementById("startTourBtn");
//    if (startBtn) startBtn.addEventListener("click", startDashboardTour);

//    window.addEventListener("load", () => {
//        const tutorialShown = localStorage.getItem("dashboardTutorialShown");

//        if (!tutorialShown) {
//            Swal.fire({
//                title: 'Bienvenido',
//                text: '¿Quieres iniciar el tutorial de la aplicación?',
//                showCancelButton: true,
//                confirmButtonText: 'Sí, iniciar',
//                cancelButtonText: 'Saltar tutorial',
//                customClass: {
//                    confirmButton: 'btn btn-dark btn-fixed-width me-2',
//                    cancelButton: 'btn btn-outline-secondary btn-fixed-width'
//                },
//                buttonsStyling: false
//            }).then((result) => {
//                if (result.isConfirmed) {
//                    startDashboardTour();
//                }
//                localStorage.setItem("dashboardTutorialShown", "true");
//            });
//        }
//    });
//});

//function startDashboardTour() {
//    const steps = [
//        { element: "#card-filtros", intro: "Filtros: selecciona fecha y monto base para actualizar las tasas." },
//        { element: "#card-moneda", intro: "Moneda principal: establece la divisa con la que quieres trabajar." },
//        { element: "#card-favoritas", intro: "Favoritas: agrega monedas que consultes frecuentemente." },
//        { element: "th:nth-child(2)", intro: "Tasa de Cambio: muestra la tasa actual de la moneda." },
//        { element: "th:nth-child(3)", intro: "Conversión: muestra el cálculo con el monto ingresado." },
//        { element: "th:nth-child(4)", intro: "Acción: permite remover la moneda de tus favoritos." }
//    ];

//    introJs().setOptions({
//        steps: steps,
//        tooltipClass: 'customIntro',
//        showStepNumbers: false,
//        showBullets: false,
//        nextLabel: 'Siguiente',
//        prevLabel: 'Anterior',
//        skipLabel: 'Saltar',
//        doneLabel: 'Finalizar',
//        exitOnOverlayClick: false,
//        overlayOpacity: 0.45
//    }).start();
//}

document.addEventListener("DOMContentLoaded", () => {
    const startBtn = document.getElementById("startTourBtn");
    if (startBtn) startBtn.addEventListener("click", startDashboardTour);

    window.addEventListener("load", () => {
        const tutorialShown = localStorage.getItem("dashboardTutorialShown");

        if (!tutorialShown) {
            Swal.fire({
                title: 'Bienvenido',
                text: '¿Quieres iniciar el tutorial de la aplicación?',
                showCancelButton: true,
                confirmButtonText: 'Sí, iniciar',
                cancelButtonText: 'Saltar tutorial',
                customClass: {
                    confirmButton: 'btn btn-dark btn-fixed-width me-2',
                    cancelButton: 'btn btn-outline-secondary btn-fixed-width'
                },
                buttonsStyling: false
            }).then((result) => {
                if (result.isConfirmed) {
                    startDashboardTour();
                }
                localStorage.setItem("dashboardTutorialShown", "true");
            });
        }
    });
});

function startDashboardTour() {
    const steps = [
        { element: "#card-filtros", intro: "Filtros: selecciona fecha y monto base para actualizar las tasas." },
        { element: "#card-moneda", intro: "Moneda principal: establece la divisa con la que quieres trabajar." },
        { element: "#card-favoritas", intro: "Favoritas: agrega monedas que consultes frecuentemente." },
        { element: "th:nth-child(2)", intro: "Tasa de Cambio: muestra la tasa actual de la moneda." },
        { element: "th:nth-child(3)", intro: "Conversión: muestra el cálculo con el monto ingresado." },
        { element: "th:nth-child(4)", intro: "Acción: permite remover la moneda de tus favoritos." }
    ];

    introJs()
        .setOptions({
            steps: steps,
            tooltipClass: 'customIntro',
            showStepNumbers: false,
            showBullets: false,
            nextLabel: 'Siguiente',
            prevLabel: 'Anterior',
            skipLabel: 'Saltar',
            doneLabel: 'Finalizar',
            exitOnOverlayClick: false,
            overlayOpacity: 0.45,
            scrollToElement: true,
            position: 'auto' 
        })
        .onafterchange(function (targetElement) {
            targetElement.scrollIntoView({ behavior: 'smooth', block: 'center' });
        })
        .start();
}
