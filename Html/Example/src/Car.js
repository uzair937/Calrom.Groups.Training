export class Car {
    constructor(model) {
        this.model = model;
    }

    drive() {
        console.log(`Beep Beep, I'm in my mums ${this.model}`);
    }
}