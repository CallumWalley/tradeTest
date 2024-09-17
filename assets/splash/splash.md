## Image Manager

Rather than elements having specific paths to specific images, images should be assigned weighted tags, and ingested at compile time.
Elements then select an image based on how well the weighted tags match.


e.g. image1.png -> [{space:100, industrial: 50, ships: 10}]