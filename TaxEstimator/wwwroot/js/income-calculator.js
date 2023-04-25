var addContributionBtn = document.getElementById("add-contribution-btn");

addContributionBtn.addEventListener("click", () => {
    let contributionBlocks = document.getElementsByClassName("hidden-contribution-block");

    if(contributionBlocks.length > 0)
    { 
        contributionBlocks[0].classList.remove("d-none");
        contributionBlocks[0].classList.remove("hidden-contribution-block");

        console.log(contributionBlocks[0]);

    }

});