export class Customer {
	customerName: string;
	id: number;
}

export class HallRequestRoom {
	hallRoom: Room;
	isPrimaryChoice: boolean;
}

export class Event {
	customer: Customer;
	hallRequestRooms: Array<HallRequestRoom>;
}

export class Room {
	id: number;
	roomName: string;
}