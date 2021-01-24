export interface Location {
    latitude: number;
    longitude: number;
}

export interface Polygon {
    locations: Location[];
}

export interface Line {
    location1: Location;
    location2: Location;
}

export interface Address {
    prefecture: string;
    city: string | null;
    ward: string | null;
    town: string | null;
    polygons: Polygon[];
}
