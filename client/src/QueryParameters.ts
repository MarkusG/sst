export class QueryParameters {
  pageSize?: number;
  page?: number = 1;
  sortField?: string;
  sortDirection?: "up" | "down";
  from?: Date;
  to?: Date;

  constructor(init?: Partial<QueryParameters>) {
    Object.assign(this, init);
  }

  toString(): string {
    const builder = [];

    if (this.pageSize) {
      builder.push(`?pageSize=${this.pageSize}`);
    }

    if (this.page) {
      const paramCharacter = builder.length === 0 ? "?" : "&";
      builder.push(`${paramCharacter}page=${this.page}`);
    }

    if (this.sortField) {
      const paramCharacter = builder.length === 0 ? "?" : "&";
      builder.push(`${paramCharacter}sortField=${this.sortField}`);
    }

    if (this.sortDirection) {
      const paramCharacter = builder.length === 0 ? "?" : "&";
      builder.push(`${paramCharacter}sortDirection=${this.sortDirection}`);
    }

    if (this.from) {
      const paramCharacter = builder.length === 0 ? "?" : "&";
      builder.push(
        `${paramCharacter}from=${encodeURIComponent(new Date(this.from).toISOString())}`,
      );
    }

    if (this.to) {
      const paramCharacter = builder.length === 0 ? "?" : "&";
      builder.push(
        `${paramCharacter}to=${encodeURIComponent(new Date(this.to).toISOString())}`,
      );
    }

    if (this.from || this.to) {
      builder.push(`&offset=${new Date().getTimezoneOffset()}`);
    }

    return builder.join("");
  }
}
