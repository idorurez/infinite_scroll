# Infinite Scroll

## A very basic, infinite scroll for a finite set of labeled buttons

I initially started this by referencing my favorite author:

http://gameprogrammingpatterns.com/object-pool.html

... but I quickly veered away. The above suggested implementaton would have saved me some memory/space and some headaches.

## Thoughts

* The (simple) object pool was by far the easiest part.
* Getting the recycled objects to move to the right place during reinitialization took the longest time to resolve, but literally 5 minutes to implement using a List data structure to keep track of order.
