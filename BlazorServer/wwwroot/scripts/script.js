window.sayHello = (value) => {
    alert(value);
    console.log(value);
}

window.getName = () => {
    return prompt("Qual è il tuo nome?");
}

//call NET static method

window.getDataFromNet = () => {
    //setInterval(() => {
    DotNet.invokeMethodAsync("BlazorServer", "GetData")
        .then(data => {
            console.log(data);
        })
    //}, 1000)
}

//call NET method in class

class DotNetToJSSample {
    static dotNetInstance;

    static setDotNetInstance(instance) {
        DotNetToJSSample.dotNetInstance = instance;
    }

    static getData() {
        DotNetToJSSample.dotNetInstance.invokeMethodAsync("GetDataAsync").then(data => {
            console.log(data);
        });
    }

    static getWelcomeMessage() {
        console.log(DotNetToJSSample.dotNetInstance);
        DotNetToJSSample.dotNetInstance.invokeMethodAsync("GetWelcomeMessage").then(data => {
            alert(data);
        });
    }
}

window.DotNetToJSSample = DotNetToJSSample;