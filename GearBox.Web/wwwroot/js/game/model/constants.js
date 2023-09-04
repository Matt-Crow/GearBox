/**
 * While I would prefer not to have a dependency-magnet like this, it is 
 * significantly less code than having the server pass all of its constants
 * to the client, then loading those constants on the client every time they're
 * needed.
 */

/**
 * the number of pixels along the side of each tile
 */
export const PIXELS_PER_TILE = 100; 