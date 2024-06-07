
let main = $("#main");
let main_height = $("#main").height();
let body = $("body");
let body_height = parseInt($("body").height());
let speed = 1;
let moving = "down";

setInterval(function () {
    if (moving == "down") {
        let top = parseInt(main.css("top")) + 1;

        main.css("top", top + "px");
    } else {
        let top = parseInt(main.css("top")) - 1;

        main.css("top", top + "px");
    }
    
    let new_top = parseInt(main.css("top"));

    if (new_top > (body_height - main_height)) {
        moving = "up";
    } 

    if (new_top < 1) {
        moving = "down";
    }
}, 20);