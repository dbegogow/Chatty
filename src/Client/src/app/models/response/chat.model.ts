import { ChatUser } from "./chat-user.model";
import { Message } from "./message.model";

export interface Chat {
    id: string;
    users: ChatUser[];
    messages: Message[];
}