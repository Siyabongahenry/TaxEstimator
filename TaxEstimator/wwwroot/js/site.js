//display spinner
const processingInput = (btn, spinnerId) => {

    let spinner = document.getElementById(spinnerId);
    spinner.classList.remove("d-none");

    btn.classList.add("d-none");
  
}
//payslip printing
function printElem(elemId) {

    var elem = document.getElementById(elemId);
    
    popup(elem.innerHTML);


}
function popup(data) {
    var mywindow = window.open('', 'new div', 'height=400,width=600');
    mywindow.document.write('<html><head><title></title>');
    mywindow.document.write('<link rel="stylesheet" href="./css/site.css" type="text/css" />');
    mywindow.document.write('<link rel="stylesheet" href"./lib/bootstrap/dist/css/bootstrap.min.css" type="text/css"  media="all"/>');
    mywindow.document.write('</head><body >');
    mywindow.document.write(data);
    mywindow.document.write('</body></html>');
    mywindow.document.close();
    mywindow.focus();
    setTimeout(function () { mywindow.print(); }, 1000);
    //mywindow.close();

    return true;
}
//income UIF Calculator
document.getElementById("Income").addEventListener("input", (e) => {

    let UIFinput = document.getElementById("Contributions_UIF");

    let income = parseFloat(e.target.value);

    let UIF = ((1 / 100) * income);
    UIF = UIF > 177.12 ? 177.12 : UIF;
    UIFinput.value = isNaN(UIF) ? 0 : UIF;

});

//contributions display
document.getElementById("contributionsCollapseBtn").addEventListener("click", (e) => {

    let openText = "+ Add contributions(UIF, Provident Fund, Travel Allowance)";
    let closeText = "- Remove contributions(UIF, Provident Fund, Travel Allowance)";
    
    e.target.innerText = e.target.innerText === closeText ? openText : closeText;
})











