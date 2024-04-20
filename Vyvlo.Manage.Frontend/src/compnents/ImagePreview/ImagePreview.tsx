import { Button, Card } from '@fluentui/react-components';
import  { useState } from 'react';

const ImagePreview = ({ src,remove,index }: { src: string,remove:(id:number)=>void,index:number }) => {
    const [isHovered, setIsHovered] = useState(false);

    const handleMouseEnter = () => {
        setIsHovered(true);
    };

    const handleMouseLeave = () => {
        setIsHovered(false);
    };

    const handleRemoveClick = () => {
        remove(index)
    };

    return (
        <div style={{ position: 'relative', display: 'inline-block', margin: '5px' }}>
            <div style={{ display: 'flex', flexDirection: 'column' }}>

                <img
                    src={src}
                    alt="Preview"
                    style={{ width: '100px', height: '100px' }}
                    onMouseEnter={handleMouseEnter}
                    onMouseLeave={handleMouseLeave}
                />
                <Button
                    onClick={handleRemoveClick}
                    style={{
                        marginTop: '5px',
                        zIndex: 0,
                    }}
                >
                    Remove
                </Button>
            </div>
            {isHovered && (
                <Card
                    style={{
                        position: 'absolute',
                        top: '0',
                        left: '110%',
                        width: '200px',
                        height: '200px',
                        backgroundColor: 'rgba(255, 255, 255, 0.9)',
                        border: '1px solid #ccc',
                        borderRadius: '5px',
                        padding: '5px',
                        zIndex: 1,
                    }}
                >
                    <img
                        src={src}
                        alt="Preview"
                        style={{ width: '100%', height: '100%', objectFit: 'cover', borderRadius: '5px' }}
                    />

                </Card>
            )}


        </div>
    );
};

export default ImagePreview;
