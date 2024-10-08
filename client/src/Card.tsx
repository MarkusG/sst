import { PropsWithChildren } from "react";

interface CardProps {
    className?: string
}

export default function Card(props: PropsWithChildren<CardProps>) {
    return (
        <div className={`bg-white p-2 rounded shadow ${props.className}`}>
            {props.children}
        </div>
    );
}
