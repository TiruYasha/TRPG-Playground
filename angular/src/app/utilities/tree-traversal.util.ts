type getChildren = (item: any) => any[];
type compare = (id: any, item: any) => boolean;

export class TreeTraversal {
    public static findChild<ID, ITEM>(id: ID, items: ITEM[], getChildren: getChildren, compare: compare): ITEM | null {
        for (let item of items) {
            compare(id, item);
            if (compare(id, item)) {
                return item;
            }

            const child = this.findChildImpl(id, item, getChildren, compare);

            if (child) {
                return child;
            }
        }

        return null;
    }

    private static findChildImpl<ID, ITEM>(id: ID, root: ITEM, getChildren: getChildren, compare: compare): ITEM {
        const stack: ITEM[] = []
        let node: ITEM, ii;
        stack.push(root);

        while (stack.length > 0) {
            node = stack.pop();
            const children = getChildren(node);
            if (compare(id, node)) {
                return node;
            } else if (children && children.length) {
                for (ii = 0; ii < children.length; ii += 1) {
                    stack.push(children[ii]);
                }
            }
        }

        return null;
    }
}
