import { expect } from "chai";
import { MathComponent } from "./math";

describe("MathComponent should", () => {
    it("sum 1 + 1 be 2", () => {
        const sut = new MathComponent();
        expect(sut.sum(1, 1)).eql(2); 
    });
    it("sum 1 + 2 be 3", () => {
        const sut = new MathComponent();
        expect(sut.sum(1, 2)).eql(3);
    });
    it("sum 1 + 2 be 3", () => {
        const sut = new MathComponent();
        expect(sut.sum(1, 2)).eql(3);
    });

    it("mult 4 * 2 be 8", () => {
        const sut = new MathComponent();
        expect(sut.mult(4, 2)).eql(8);
    });

});
