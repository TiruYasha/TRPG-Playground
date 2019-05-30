import { Subject } from 'rxjs';
import { OnDestroy } from '@angular/core';

export abstract class DestroySubscription implements OnDestroy {
    protected destroy = new Subject();

    ngOnDestroy(): void {
        this.destroy.next();
    }
}
