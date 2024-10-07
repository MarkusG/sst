import { useState } from "react";
import { CategoryTreeEntryResponse } from "../Contracts/Responses";
import Draggable from "../Draggable";
import Dropzone from "../Dropzone";

export type DragOverPosition = 'top' | 'mid' | 'bot' | null;

export interface DragOverEvent {
    category: CategoryTreeEntryResponse,
    position: DragOverPosition
}

export interface CategoryProps {
    entry: CategoryTreeEntryResponse,
    last: boolean,
    level: number,
    onDragStart: (e: React.DragEvent<HTMLDivElement>, data: CategoryTreeEntryResponse) => Promise<void>,
    onDragEnd: (e: React.DragEvent<HTMLDivElement>, data: CategoryTreeEntryResponse) => Promise<void>,
    onDragOver: (e: DragOverEvent) => Promise<void>,
    isDragging: boolean,
    dragOverCategory: CategoryTreeEntryResponse | null,
    dragOverPosition: DragOverPosition
}

function Category({ entry, last, level, onDragStart, onDragEnd, onDragOver, isDragging, dragOverCategory, dragOverPosition }: CategoryProps) {
    const [open, setOpen] = useState(true);

    async function dragStart(e: React.DragEvent<HTMLDivElement>, data: CategoryTreeEntryResponse) {
        e.stopPropagation();
        await onDragStart(e, data);
    }

    async function dragEnd(e: React.DragEvent<HTMLDivElement>, data: CategoryTreeEntryResponse) {
        e.stopPropagation();
        await onDragEnd(e, data);
    }

    async function dragOver(position: DragOverPosition) {
        await onDragOver({ category: entry, position });
    }

    return (
        <Draggable data={entry} onDragStart={dragStart} onDragEnd={dragEnd}>
            <div className="cursor-grab flex flex-col">
                {isDragging &&
                    <div className={`w-full h-[.2rem] ${dragOverCategory?.id === entry.id && dragOverPosition === 'top' ? 'bg-primary-200' : ''}`} style={{ marginLeft: `${(level + 1) * .5}rem` }}>
                        <Dropzone className="w-full h-full" onDragOver={async (_, __) => await dragOver('top')}/>
                    </div>
                }
                <div className={`relative ${(dragOverCategory?.id === entry.id && dragOverPosition === 'mid') ? 'bg-primary-200' : ''}`} style={{ paddingLeft: `${(level + 1) * .5}rem` }}>
                    <button
                        className="disabled:cursor-[inherit]"
                        disabled={entry.subcategories.length === 0}
                        onClick={() => setOpen(!open)}>
                        <span>{entry.name}</span>
                        {entry.subcategories.length > 0 && <>
                            { open
                                ? <i className="ml-1 fa fa-xs fa-chevron-down"></i>
                                : <i className="ml-1 fa fa-xs fa-chevron-right"></i>
                            }
                            </>
                        }
                    </button>
                    {isDragging &&
                        <>
                            <Dropzone className={`absolute top-0 left-0 w-full h-full z-10`}
                                onDragOver={async (_, __) => await dragOver('mid')}>
                            </Dropzone>
                        </>
                    }
                </div>
                {entry.subcategories.length > 0 &&
                    <div className={!open ? 'hidden' : ''}>
                        {entry.subcategories.map(c => <Category
                                                 key={c.id}
                                                 entry={c}
                                                 last={c.position === entry.subcategories.length}
                                                 level={level + 1}
                                                 isDragging={isDragging}
                                                 onDragStart={dragStart}
                                                 onDragEnd={dragEnd}
                                                 onDragOver={async (e) => await onDragOver(e)}
                                                 dragOverCategory={dragOverCategory}
                                                 dragOverPosition={dragOverPosition}/>)}
                    </div>
                }
            </div>
            {isDragging && last &&
                <div className={`w-full h-[.2rem] ${dragOverCategory?.id === entry.id && dragOverPosition === 'bot' ? 'bg-primary-200' : ''}`} style={{ marginLeft: `${(level + 1) * .5}rem` }}>
                    <Dropzone className="w-full h-full" onDragOver={async (_, __) => await dragOver('bot')}/>
                </div>
            }
        </Draggable>
    );
}

export default Category;
