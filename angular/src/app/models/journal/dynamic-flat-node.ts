export class DynamicFlatNode<T> {
    constructor(public item: T, public level = 1, public expandable = false,
        public isLoading = false) { }
}