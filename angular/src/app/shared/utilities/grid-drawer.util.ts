import { Application, Graphics } from 'pixi.js';

export abstract class GridDrawer {
    public static drawGrid(gridSizeInPixels: number, application: Application) {
        const width = application.renderer.width;
        const height = application.renderer.height;

        for (let j = 0; j < height; j += gridSizeInPixels) {
            const horizontalLine = new Graphics();
            horizontalLine.lineStyle(1, 0x00000, 1);
            horizontalLine.moveTo(0, j);
            horizontalLine.lineTo(width, j);
            horizontalLine.x = 0;
            horizontalLine.y = j;
            horizontalLine.zIndex = 100;
            application.stage.addChild(horizontalLine);
        }

        for (let i = 0; i < width; i += gridSizeInPixels) {
            const verticalLine = new Graphics();
            verticalLine.lineStyle(1, 0x00000, 1);
            verticalLine.moveTo(i, 0);
            verticalLine.lineTo(i, height);
            verticalLine.x = i;
            verticalLine.y = 0;
            verticalLine.zIndex = 100;
            application.stage.addChild(verticalLine);
        }
    }
}
