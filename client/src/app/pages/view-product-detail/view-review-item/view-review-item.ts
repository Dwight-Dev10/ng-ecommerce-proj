import { Component, input } from '@angular/core';
import { UserReview } from '../../../models/user-review';
import { ViewPanel } from '../../../directives/view-panel';
import { StarRating } from "../../../components/star-rating/star-rating";
import { DatePipe } from '@angular/common';

@Component({
  selector: 'app-view-review-item',
  imports: [ViewPanel, StarRating, DatePipe],
  template: ` 
    <div appViewPanel>
      <div class="flex items-start gap-4">
        <img [src]="review().userImageUrl"
          [alt]="review().UserName"
          class="w-10 h-10 rounded-full"
        />
        <div>
          <div class="text-lg font-semibold">
            {{ review().UserName }}
          </div>
          <div class="flex items-center mb-1">
            <app-star-rating [rating]="review().rating" />
            <div class="text-sm text-gray-500">{{ review().reviewDate | date: 'MMM d, yyyy' }}</div>
          </div>
          <div class="text-base font-semibold mb-1">{{review().title}}</div>
          <div class="text-sm text-gray-500">{{review().comment}}</div>
        </div>
      </div>
    </div>
  `,
  styles: ``,
})
export class ViewReviewItem {
  review = input.required<UserReview>();
}
