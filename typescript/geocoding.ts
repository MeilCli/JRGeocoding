import { Line, Polygon, Address } from "./entities";
import { Reader } from "./reader";

const mapMagnification = 10;

export async function reverseGeocoding(reader: Reader, latitude: number, longitude: number): Promise<Address | null> {
    const magnifiedLatitude = Math.floor(latitude * mapMagnification);
    const magnifiedLongitude = Math.floor(longitude * mapMagnification);
    const addresses = await reader.read(`${magnifiedLatitude}-${magnifiedLongitude}.json`);
    if (addresses == null) {
        return null;
    }
    for (const address of addresses) {
        for (const polygon of address.polygons) {
            if (isInside(latitude, longitude, polygon)) {
                return address;
            }
        }
    }
    return null;
}

// Winding Number Algorithm
// ref: https://www.nttpc.co.jp/technology/number_algorithm.html
// ref: https://gist.github.com/vlasky/d0d1d97af30af3191fc214beaf379acc
function isInside(latitude: number, longitude: number, polygon: Polygon): boolean {
    let wn = 0;
    const lines = toLines(polygon);
    for (const line of lines) {
        if (line.location1.latitude <= latitude) {
            if (latitude < line.location2.latitude) {
                const left =
                    (line.location2.longitude - line.location1.longitude) * (latitude - line.location1.latitude);
                const right =
                    (longitude - line.location1.longitude) * (line.location2.latitude - line.location1.latitude);
                if (0 < left - right) {
                    wn += 1;
                }
            }
        } else {
            if (line.location2.latitude <= latitude) {
                const left =
                    (line.location2.longitude - line.location1.longitude) * (latitude - line.location1.latitude);
                const right =
                    (longitude - line.location1.longitude) * (line.location2.latitude - line.location1.latitude);
                if (left - right < 0) {
                    wn -= 1;
                }
            }
        }
    }
    return 1 <= wn;
}

function toLines(polygon: Polygon): Line[] {
    const result: Line[] = [];
    if (polygon.locations.length == 0) {
        return result;
    }

    for (let i = 0; i < polygon.locations.length; i++) {
        if (i < polygon.locations.length - 1) {
            result.push({ location1: polygon.locations[i], location2: polygon.locations[i + 1] });
        } else {
            result.push({ location1: polygon.locations[i], location2: polygon.locations[0] });
        }
    }

    return result;
}
