import { PropsWithChildren } from "react";

interface DropzoneProps<T> {
    className?: string,
    onDrop?: (e: React.DragEvent<HTMLDivElement>, data: T) => Promise<void>,
    onDragOver?: (e: React.DragEvent<HTMLDivElement>, data: T) => Promise<void>,
    onDragLeave?: (e: React.DragEvent<HTMLDivElement>, data: T) => Promise<void>
}

export default function Dropzone<T>(props: PropsWithChildren<DropzoneProps<T>>) {
    async function onDrop(e: React.DragEvent<HTMLDivElement>) {
        e.preventDefault();
        const data = JSON.parse(e.dataTransfer.getData("application/json")) as T;
        if (props.onDrop) {
            await props.onDrop(e, data);
        }
    }

    async function onDragOver(e: React.DragEvent<HTMLDivElement>) {
        e.preventDefault();
        if (props.onDragOver) {
            const data = JSON.parse(e.dataTransfer.getData("application/json")) as T;
            await props.onDragOver(e, data);
        }
    }

    async function onDragLeave(e: React.DragEvent<HTMLDivElement>) {
        e.preventDefault();
        if (props.onDragLeave) {
            const data = JSON.parse(e.dataTransfer.getData("application/json")) as T;
            await props.onDragLeave(e, data);
        }
    }

    return (
        <div className={props.className} onDrop={onDrop} onDragOver={onDragOver} onDragLeave={onDragLeave}>
            {props.children}
        </div>
    )
}
