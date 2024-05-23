var container;
var canvas;
var timer = 0;
var loadingContainer = null;
var loadingText = null;
var progressbar;

var runtimeInitialized = false;

function updateLoadingText(timestamp) {

    if(runtimeInitialized) {
        loadingContainer.style.display="none";
        return;
    }

    var timestampSeconds = timestamp / 1000;
    var numberOfLoadingDots = (timestampSeconds % 4);
    var dotsText = "";
    for(var i= 0; i < numberOfLoadingDots; i++) {
        dotsText = dotsText + ".";
    }
    loadingText.textContent = "Loading"+dotsText;
    window.requestAnimationFrame(updateLoadingText);
}

function OnLoadProgress(e){

    //ロードの進捗
    if(progressbar!=null){
        progressbar.setAttribute("value", e);
    }
}

function OnRuntimeIntialized() {
    runtimeInitialized = true;
    canvas = document.querySelector("#unity-canvas");
    container = document.querySelector("#gameContainer");
    container.style.width = window.innerWidth + 'px';
    container.style.height = window.innerHeight + 'px';
    container.style.overflow = 'hidden';
    
    container.appendChild(canvas);
    document.body.appendChild(container);

    if(canvas != null) {
        var canvasSize = getCanvasSize();
        canvas.width = canvasSize.width;
        canvas.height = canvasSize.height;
    }
    
    document.body.style.margin = '0px'
}
//初期化
function init() {
    progressbar = document.getElementById("progress_bar");

    loadingContainer = document.body.querySelector("#loadingContainer");
    loadingText = loadingContainer.querySelector("span");
    window.requestAnimationFrame(updateLoadingText);
}

function getCanvasSize() {
    var windowWidth = window.innerWidth;
    var windowHeight = window.innerHeight;
    return {width:windowWidth,height:windowHeight};
}

//サイズ変更処理
function resize() {
    container.style.width = window.innerWidth + 'px';
    container.style.height = window.innerHeight + 'px';
    canvas.width = window.innerWidth * window.devicePixelRatio;
    canvas.height = window.innerHeight * window.devicePixelRatio;
}

window.onload = function () {
    init();
    resize();
};

//ブラウザの大きさが変わった時に行う処理
(function () {
    var timer = 0;
    window.onresize = function () {
        if (timer > 0) {
            clearTimeout(timer);
        }
        timer = setTimeout(function () {
            resize();
        }, 200);
    };
}());
