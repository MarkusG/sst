export interface TimestampProps {
    ts: Date
}

export default function Timestamp({ ts }: TimestampProps) {
    // we have to do this apparently?
    const date = new Date(ts);
    const year = date.getFullYear();
    const month = (date.getMonth() + 1).toString().padStart(2, '0');
    const day = date.getDate().toString().padStart(2, '0');

    const hour = date.getHours().toString().padStart(2, '0');
    const minute = date.getMinutes().toString().padStart(2, '0');

    return (<>{year}-{month}-{day} {hour}:{minute}</>);
}
